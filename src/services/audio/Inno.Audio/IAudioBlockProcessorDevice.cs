namespace Inno.Audio;

/// <summary>
/// Admits external sample processing on devices that provide buffered mixer output.
/// </summary>
public interface IAudioBlockProcessorDevice
{
    /// <summary>
    /// Appends a buffered processor after the current effects on a bus.
    /// </summary>
    /// <param name="bus">
    /// The live bus belonging to this device.
    /// </param>
    /// <param name="configuration">
    /// The immutable configuration for this effect.
    /// </param>
    /// <param name="processor">
    /// The processor transferred to the bus only on success.
    /// </param>
    /// <returns>
    /// Whether the device accepted ownership of the processor.
    /// </returns>
    bool AddBlockProcessor(
        AudioBusHandle bus,
        AudioProcessorConfiguration configuration,
        AudioBlockProcessor processor
    );
}
