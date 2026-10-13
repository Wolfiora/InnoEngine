using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Cli;

/// <summary>
/// Prepares immutable Build-tool deployments through the application's fixed-input publication boundary.
/// </summary>
public static partial class BuildApplication
{
    /// <summary>
    /// Publishes a verified Build-tool deployment before the development entry loads it for the process lifetime.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute checkout owning the tool's evaluated source graph and caches.
    /// </param>
    /// <param name="configuration">
    /// The explicit Debug or Release configuration used to prepare this deployment.
    /// </param>
    /// <param name="dotnetHost">
    /// The selected managed SDK executable used to verify current inputs.
    /// </param>
    /// <param name="assemblyPath">
    /// The built CLI assembly in an exclusively owned, freshly prepared SDK output directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels copying or ownership acquisition before publication.
    /// </param>
    /// <returns>
    /// The immutable assembly path, protected by a registered reader for the current process lifetime.
    /// </returns>
    /// <remarks>
    /// The bootstrap deployment provides this preparation implementation. The accepted tool is compiled
    /// again from operation-fixed repository inputs before publication, never from a mutable checkout.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// A path or configuration is invalid or outside the checkout.
    /// </exception>
    /// <exception cref="IOException">
    /// Input or output files cannot be verified, copied, or published.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The evaluated inputs or deployment change during publication.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Another reader or producer retains mutation ownership beyond the bounded wait.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation is canceled before commit.
    /// </exception>
    public static async Task<string> PublishRuntimeAsync(
        string engineRoot,
        string configuration,
        string dotnetHost,
        string assemblyPath,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        ToolchainEnvironment.ValidateConfiguration(configuration.ToLowerInvariant());
        string root = Path.GetFullPath(engineRoot);
        string assembly = PathBoundary.RequireUnlinkedPath(root, assemblyPath);
        string source = Path.GetDirectoryName(assembly)!;
        string workspaceRoot = Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces");
        PathBoundary.RequireUnlinkedPath(workspaceRoot, source);
        string project = Path.Combine(root, "build", "cli", "Inno.Build.Cli", "Inno.Build.Cli.csproj");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnetHost, project, cancellationToken).ConfigureAwait(false);
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
            new NativeBuildContext(root, configuration.ToLowerInvariant()), sdk, project,
            new Dictionary<string, string> { ["Configuration"] = configuration }, cancellationToken).ConfigureAwait(false);
        using ManagedBuildExecution execution = await snapshot.CreateExecutionAsync(cancellationToken).ConfigureAwait(false);
        string workspaceIdentity = NativeBuildFingerprint.Create(["build-tool-workspace", execution.project, sdk.sdkIdentity, configuration], []);
        string compilerWorkspace = Path.Combine(workspaceRoot, workspaceIdentity);
        var workspaces = new BuildCachePublisher(workspaceRoot);
        using FileLease workspaceUse = await workspaces.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        string bootstrapWorkspace = Path.Combine(workspaceRoot,
            Path.GetRelativePath(workspaceRoot, source).Split(Path.DirectorySeparatorChar)[0]);
        workspaces.RecordProcessUse(bootstrapWorkspace, bootstrapWorkspace + ".lock", [], cancellationToken);
        using FileLease compilerOwnership = await FileLease.AcquireAsync(compilerWorkspace + ".lock",
            TimeSpan.FromMinutes(10), cancellationToken).ConfigureAwait(false);
        string preparedOutput = Path.Combine(compilerWorkspace, "Runtime");
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "build", execution.project, "--configuration", configuration, "-m:4", "-nodeReuse:false",
                "-p:InnoBuildCliOutputPath=" + preparedOutput + Path.DirectorySeparatorChar],
            execution.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(sdk.hostPath)).ConfigureAwait(false);
        await execution.VerifyAsync(cancellationToken).ConfigureAwait(false);
        await snapshot.VerifyAsync(cancellationToken).ConfigureAwait(false);
        workspaces.Record(compilerWorkspace, compilerWorkspace + ".lock", [], null, null, cancellationToken);
        source = Path.Combine(preparedOutput, "net9.0");
        assembly = Path.Combine(source, "Inno.Build.Cli.dll");
        string inputs = snapshot.fingerprint;
        IReadOnlyDictionary<string, string> files = ReadDeployment(source, cancellationToken);
        string fingerprint = NativeBuildFingerprint.Create(files.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
            .Select(static pair => pair.Key + "=" + pair.Value).Prepend(inputs), []);
        string cacheRoot = Path.Combine(root, "artifacts", "build-tools", "cli", "hosts");
        string destination = Path.Combine(cacheRoot, configuration.ToLowerInvariant(), fingerprint);
        var cache = new BuildCachePublisher(cacheRoot);
        using FileLease activity = await cache.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        BuildArtifactReadLease? reading = await TryReadRuntimeAsync(destination, fingerprint, cancellationToken).ConfigureAwait(false);
        try
        {
            if (reading is null)
            {
                using (BuildArtifactWriteLease writing = await BuildArtifactWriteLease.AcquireAsync(destination, cancellationToken).ConfigureAwait(false))
                {
                    if (!BuildArtifactManifest.IsComplete(destination, fingerprint, ["Outputs"]))
                    {
                        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
                        string outputs = Path.Combine(staging, "Outputs");
                        Directory.CreateDirectory(outputs);
                        try
                        {
                            foreach (string relative in files.Keys)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                string target = PathBoundary.Resolve(outputs, relative);
                                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                                File.Copy(PathBoundary.Resolve(source, relative), target);
                            }
                            ValidateDeployment(files, ReadDeployment(source, cancellationToken));
                            ValidateDeployment(files, ReadDeployment(outputs, cancellationToken));
                            await execution.VerifyAsync(cancellationToken).ConfigureAwait(false);
                            await snapshot.VerifyAsync(cancellationToken).ConfigureAwait(false);
                            BuildArtifactManifest.Write(staging, fingerprint, ["Outputs"]);
                            cancellationToken.ThrowIfCancellationRequested();
                            if (Directory.Exists(destination))
                            {
                                try
                                {
                                    AtomicDirectory.Install(staging, destination);
                                }
                                catch (DirectoryCleanupException cleanupFailure)
                                {
                                    Console.Error.WriteLine(cleanupFailure.Message);
                                }
                            }
                            else
                                AtomicDirectory.Publish(staging, destination, cancellationToken);
                        }
                        finally
                        {
                            if (Directory.Exists(staging))
                                Directory.Delete(PathBoundary.RequireUnlinkedPath(cacheRoot, staging), recursive: true);
                        }
                    }
                }
                reading = await BuildArtifactReadLease.AcquireAsync(destination, fingerprint, ["Outputs"], cancellationToken)
                    .ConfigureAwait(false);
            }
            cache.Record(destination, destination + ".read.lock", [], configuration, null, cancellationToken);
            string readersRoot = Path.Combine(root, "artifacts", "build-tools", "cli", "readers");
            string reader = Path.Combine(readersRoot, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(reader);
            new BuildCachePublisher(readersRoot).RecordProcessUse(reader, reader + ".lock", [destination], cancellationToken);
            return Path.Combine(destination, "Outputs", Path.GetFileName(assembly));
        }
        finally
        {
            reading?.Dispose();
        }
    }

    private static async ValueTask<BuildArtifactReadLease?> TryReadRuntimeAsync(
        string directory,
        string fingerprint,
        CancellationToken cancellationToken
    ) {
        try
        {
            return await BuildArtifactReadLease.AcquireAsync(directory, fingerprint, ["Outputs"], cancellationToken)
                .ConfigureAwait(false);
        }
        catch (InvalidDataException)
        {
            return null;
        }
    }

    private static IReadOnlyDictionary<string, string> ReadDeployment(
        string directory,
        CancellationToken cancellation
    ) {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string file in PathBoundary.EnumerateFiles(directory).Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            using FileStream stream = File.OpenRead(file);
            result.Add(Path.GetRelativePath(directory, file), Convert.ToHexStringLower(SHA256.HashData(stream)));
        }
        if (result.Count == 0)
            throw new InvalidDataException("The prepared Build-tool deployment is empty.");
        return result;
    }

    private static void ValidateDeployment(
        IReadOnlyDictionary<string, string> expected,
        IReadOnlyDictionary<string, string> actual
    ) {
        if (expected.Count != actual.Count || actual.Any(pair => !expected.TryGetValue(pair.Key, out string? hash) || hash != pair.Value))
            throw new InvalidOperationException("The Build-tool deployment changed during publication.");
    }
}
