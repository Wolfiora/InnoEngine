namespace Inno.Audio;

/// <summary>
/// Selects the distance attenuation model for one spatial voice.
/// </summary>
public enum AudioDistanceModel
{
    /// <summary>
    /// Disables distance attenuation while retaining spatial direction.
    /// </summary>
    None,
    /// <summary>
    /// Uses inverse-distance attenuation.
    /// </summary>
    Inverse,
    /// <summary>
    /// Uses linear-distance attenuation.
    /// </summary>
    Linear,
    /// <summary>
    /// Uses exponential-distance attenuation.
    /// </summary>
    Exponential
}
