using System;
using System.Collections.Generic;
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
    /// The exact project-selected SDK, managed entry assembly and host; missing SDKs fail explicitly.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The selected host, prepared entry project or selected SDK entry assembly does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected host cannot resolve a valid SDK identity and absolute base path.
    /// </exception>
    public static ValueTask<DotNetSdkDescriptor> ResolveAsync(
        string hostPath,
        string projectPath,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        string project = Path.GetFullPath(projectPath);
        if (!File.Exists(project))
            throw new FileNotFoundException("The managed entry project is absent.", project);
        return ResolveDirectoryAsync(hostPath, Path.GetDirectoryName(project)!, cancellationToken);
    }

    /// <summary>
    /// Selects the SDK from an authoring owner's directory before any temporary evaluation project is created.
    /// </summary>
    /// <param name="hostPath">
    /// The managed host explicitly selected by the composition root.
    /// </param>
    /// <param name="ownerDirectory">
    /// The existing source owner whose ancestor global.json rules govern the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels discovery and drains the SDK process.
    /// </param>
    /// <returns>
    /// The selected SDK and fixed managed CLI path, without probing reference pack directories.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    /// The source owner directory does not exist.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The host or selected SDK CLI is missing.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The host cannot resolve the source owner's requested SDK.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels SDK discovery.
    /// </exception>
    public static async ValueTask<DotNetSdkDescriptor> ResolveDirectoryAsync(
        string hostPath,
        string ownerDirectory,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerDirectory);
        string directory = Path.GetFullPath(ownerDirectory);
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"The managed SDK source owner is absent: '{directory}'.");
        hostPath = ToolchainEnvironment.ResolveExecutable(hostPath);
        string information = await ToolchainEnvironment.CaptureOutputAsync(hostPath, ["--info"],
            directory, cancellationToken,
            DotNetSdkEnvironment.Create(hostPath, new Dictionary<string, string>
            {
                ["DOTNET_CLI_UI_LANGUAGE"] = "en-US"
            })).ConfigureAwait(false);
        string identity = string.Empty;
        string sdkDirectory = string.Empty;
        foreach (string outputLine in information.Split('\n'))
        {
            string line = outputLine.Trim();
            if (identity.Length == 0 && line.StartsWith("Version:", StringComparison.Ordinal))
                identity = line["Version:".Length..].Trim();
            if (line.StartsWith("Base Path:", StringComparison.Ordinal))
                sdkDirectory = line["Base Path:".Length..].Trim();
        }
        string numericVersion = identity.Split('-', 2)[0];
        if (!Version.TryParse(numericVersion, out _) || !Path.IsPathFullyQualified(sdkDirectory))
            throw new InvalidOperationException("The managed host did not resolve a valid project SDK identity and base path.");
        string cli = Path.Combine(sdkDirectory, "dotnet.dll");
        if (!File.Exists(cli))
            throw new FileNotFoundException("The selected SDK managed CLI entry assembly is unavailable.", cli);
        return new DotNetSdkDescriptor(hostPath, identity, cli);
    }
}
