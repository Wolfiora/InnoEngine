namespace Inno.Input;

/// <summary>
/// Admits device instances into one session's ordered input stream.
/// </summary>
public interface IInputDeviceSource
{
    /// <summary>
    /// Connects one instance of the named device protocol on the input owner thread.
    /// </summary>
    /// <param name="typeID">
    /// A stable, case-sensitive identifier shared by devices exposing the same controls.
    /// </param>
    /// <returns>
    /// A connection owned by the provider. Dispose it when the device disconnects or the provider retires.
    /// </returns>
    InputDeviceConnection ConnectDevice(string typeID);
}
