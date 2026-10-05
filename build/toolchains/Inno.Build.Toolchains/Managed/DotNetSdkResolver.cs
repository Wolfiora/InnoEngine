using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Resolves the managed SDK using the prepared entry project's directory and normal global.json rules.
/// </summary>
public static class DotNetSdkResolver
{
    /// <summary>
    /// Asks the selected host to resolve its SDK from the project location without guessing installed versions.
    /// </summary>
    /// <param name="hostPath">
    /// The .NET executable selected by the build composition root.
    /// </param>
    /// <param name="projectPath">
    /// The prepared entry project whose directory controls SDK selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels SDK discovery and drains its child process.
    /// </param>
    /// <returns>
    /// The exact project-selected SDK and host; missing projects or SDKs fail explicitly.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The prepared entry project does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected host cannot resolve a valid SDK identity.
    /// </exception>
    public static async ValueTask<DotNetSdkDescriptor> ResolveAsync(
        string hostPath,
        string projectPath,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        string project = Path.GetFullPath(projectPath);
        if (!File.Exists(project))
            throw new FileNotFoundException("The managed entry project is absent.", project);
        string identity = (await ToolchainEnvironment.CaptureOutputAsync(hostPath, ["--version"],
            Path.GetDirectoryName(project)!, cancellationToken).ConfigureAwait(false)).Trim();
        string numericVersion = identity.Split('-', 2)[0];
        if (!Version.TryParse(numericVersion, out _))
            throw new InvalidOperationException("The managed host did not resolve a valid project SDK identity.");
        return new DotNetSdkDescriptor(hostPath, identity);
    }
}
