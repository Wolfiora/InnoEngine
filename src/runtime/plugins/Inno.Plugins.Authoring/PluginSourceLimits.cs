namespace Inno.Plugins.Authoring;

/// <summary>
/// Controls bounded validation and archive extraction for installed Plugin packages.
/// </summary>
public sealed class PluginSourceLimits
{
    /// <summary>
    /// Gets default conservative local Plugin limits.
    /// </summary>
    public static PluginSourceLimits defaults { get; } = new();

    /// <summary>
    /// Gets or initializes the maximum number of source entries.
    /// </summary>
    public int maximumEntryCount { get; init; } = 100_000;

    /// <summary>
    /// Gets or initializes the maximum size of one source file.
    /// </summary>
    public long maximumFileBytes { get; init; } = 64L * 1024 * 1024;

    /// <summary>
    /// Gets or initializes the maximum total source size.
    /// </summary>
    public long maximumTotalBytes { get; init; } = 1024L * 1024 * 1024;

    /// <summary>
    /// Gets or initializes the maximum accepted archive uncompressed-to-compressed ratio.
    /// </summary>
    public double maximumCompressionRatio { get; init; } = 200d;

    /// <summary>
    /// Gets or initializes the maximum number of embedded dependency packages in one installation.
    /// </summary>
    public int maximumEmbeddedPluginCount { get; init; } = 256;

    /// <summary>
    /// Gets or initializes the maximum nested embedded dependency depth.
    /// </summary>
    public int maximumEmbeddedDepth { get; init; } = 16;
}
