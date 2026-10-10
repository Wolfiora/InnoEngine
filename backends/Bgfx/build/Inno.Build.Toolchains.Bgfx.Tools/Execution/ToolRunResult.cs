namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Contains the immutable result of one bgfx tool invocation.
/// </summary>
public sealed class ToolRunResult
{
    /// <summary>
    /// Creates a tool invocation result.
    /// </summary>
    /// <param name="exitCode">
    /// Native process exit code.
    /// </param>
    /// <param name="standardOutput">
    /// Captured standard output.
    /// </param>
    /// <param name="standardError">
    /// Captured standard error.
    /// </param>
    public ToolRunResult(
        int exitCode,
        string standardOutput,
        string standardError
    ) {
        this.exitCode = exitCode;
        this.standardOutput = standardOutput ?? string.Empty;
        this.standardError = standardError ?? string.Empty;
    }

    /// <summary>
    /// Gets the native process exit code.
    /// </summary>
    public int exitCode { get; }

    /// <summary>
    /// Gets captured standard output.
    /// </summary>
    public string standardOutput { get; }

    /// <summary>
    /// Gets captured standard error.
    /// </summary>
    public string standardError { get; }

    /// <summary>
    /// Gets whether the tool exited successfully.
    /// </summary>
    public bool succeeded => exitCode == 0;
}

