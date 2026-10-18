using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Managed;
using Inno.Build.Toolchains;

namespace Inno.Build.Managed.DotNet;

internal static class DotNetPublishExecutor
{
    internal static async ValueTask<ManagedDeploymentResult> PublishAsync(
        DotNetPublishRequest request,
        CancellationToken cancellationToken
    ) {
        ManagedDeploymentRequest publication = request.publication;
        if (!Directory.Exists(publication.codeInputDirectory))
            throw new DirectoryNotFoundException("The frozen managed code input directory is absent.");
        if (Directory.Exists(publication.outputDirectory)
            && Directory.EnumerateFileSystemEntries(publication.outputDirectory).Any())
            throw new InvalidOperationException("Managed publication requires an empty staging directory.");
        Directory.CreateDirectory(publication.logDirectory);
        await File.WriteAllTextAsync(Path.Combine(publication.logDirectory, "managed-sdk.txt"),
            request.sdk.sdkIdentity, cancellationToken).ConfigureAwait(false);
        await File.WriteAllTextAsync(Path.Combine(publication.logDirectory, "managed-cli.txt"),
            request.sdk.cliPath, cancellationToken).ConfigureAwait(false);
        string physicalProjectDirectory = Path.GetDirectoryName(publication.projectPath)!;
        using ToolchainWorkingDirectory workspace = await ToolchainWorkingDirectory.OpenAsync(
            physicalProjectDirectory, cancellationToken).ConfigureAwait(false);
        await File.WriteAllLinesAsync(Path.Combine(publication.logDirectory, "managed-working-directory.txt"),
            [physicalProjectDirectory, workspace.toolPath], cancellationToken).ConfigureAwait(false);
        var arguments = new List<string>
        {
            request.sdk.cliPath, "publish", Path.Combine(workspace.toolPath, Path.GetFileName(publication.projectPath)),
            "--disable-build-servers", "-m:1", "-nodeReuse:false",
            "--configuration", "Release", "--runtime", publication.runtimeIdentifier,
            "--output", publication.outputDirectory, "--nologo",
            "-p:DebugType=None", "-p:DebugSymbols=false",
            "-p:TreatWarningsAsErrors=true", "-p:TrimmerSingleWarn=false",
            "-p:InnoGameManagedRoot=" + publication.codeInputDirectory
        };
        arguments.AddRange(request.properties.Select(static property => "-p:" + property));
        await using var output = new TailLogWriter(Path.Combine(publication.logDirectory, "managed-output.log"));
        await using var error = new TailLogWriter(Path.Combine(publication.logDirectory, "managed-error.log"));
        try
        {
            await ToolchainEnvironment.RunAsync(request.sdk.hostPath, arguments,
                workspace.toolPath, cancellationToken,
                DotNetSdkEnvironment.Create(request.sdk.hostPath), output, error).ConfigureAwait(false);
        }
        catch (InvalidOperationException exception)
        {
            throw new InvalidOperationException($"Managed deployment '{request.deployment}' failed."
                + Environment.NewLine + output.tail + Environment.NewLine + error.tail, exception);
        }
        if (!Directory.Exists(publication.outputDirectory))
            throw new InvalidOperationException("The managed publisher produced no output directory.");
        string[] files = Directory.EnumerateFiles(publication.outputDirectory, "*", SearchOption.AllDirectories)
            .Select(file => Path.GetRelativePath(publication.outputDirectory, file)).Order(StringComparer.Ordinal).ToArray();
        return new ManagedDeploymentResult(request.deployment, publication.outputDirectory, request.sdk.sdkIdentity, files);
    }

    private sealed class TailLogWriter : TextWriter
    {
        private readonly StreamWriter m_log;
        private readonly Queue<string> m_tail = new();

        internal TailLogWriter(string path) => m_log = new StreamWriter(path, append: false);

        internal string tail => string.Join(Environment.NewLine, m_tail);

        /// <inheritdoc />
        public override Encoding Encoding => Encoding.UTF8;

        /// <inheritdoc />
        public override async Task WriteLineAsync(string? value)
        {
            await m_log.WriteLineAsync(value).ConfigureAwait(false);
            if (value is null)
                return;
            m_tail.Enqueue(value.Length <= 4096 ? value : value[..4096]);
            if (m_tail.Count > 100)
                m_tail.Dequeue();
        }

        /// <inheritdoc />
        public override ValueTask DisposeAsync() => m_log.DisposeAsync();
    }
}
