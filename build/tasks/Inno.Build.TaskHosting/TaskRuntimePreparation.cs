using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Text.Json;
using Inno.Core.IO;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Inno.Build.TaskHosting;

internal static class TaskRuntimePreparation
{
    internal static ITaskItem[]? ReadRequest(
        string cache,
        string request,
        string load,
        CancellationToken cancellation
    ) {
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ImmutableArtifact);
        using FileLease activity = publisher.AcquireUseAsync(cancellation).AsTask().GetAwaiter().GetResult();
        string index = Path.Combine(cache, request + ".request.json");
        if (!File.Exists(index))
            return null;
        RuntimeFile[] files;
        try
        {
            files = JsonSerializer.Deserialize<RuntimeFile[]>(File.ReadAllBytes(index)) ?? [];
            ITaskItem[] declared = CreateOutputs(files);
            files = FreezeFiles(declared);
        }
        catch (Exception error) when (error is JsonException or InvalidDataException or ArgumentException)
        {
            return null;
        }
        if (files.Length == 0)
            return null;
        string assembly = files.Single(static file => file.relative == "Inno.Build.Tasks.dll").source;
        string runtime = Path.GetDirectoryName(assembly)!;
        string publication = Path.GetDirectoryName(runtime)!;
        PathBoundary.RequireUnlinkedPath(cache, publication);
        if (Path.GetRelativePath(cache, publication) != Fingerprint(files)
            || files.Any(file => file.source != PathBoundary.Resolve(runtime, file.relative)))
            return null;
        using FileLease reading = FileLease.AcquireSharedAsync(publication + ".lock", TimeSpan.FromSeconds(30), cancellation)
            .AsTask().GetAwaiter().GetResult();
        if (!IsComplete(runtime, files, cancellation))
            return null;
        RegisterReader(load, assembly);
        return CreateOutputs(files);
    }

    internal static void RecordRequest(
        string cache,
        string request,
        ITaskItem[] outputs
    ) => AtomicFile.WriteAllBytes(Path.Combine(cache, request + ".request.json"), JsonSerializer.SerializeToUtf8Bytes(FreezeFiles(outputs)));

    private static ITaskItem[] CreateOutputs(IReadOnlyList<RuntimeFile> files) => files.Select(file =>
    {
        var item = new TaskItem(file.source);
        item.SetMetadata("RelativePath", file.relative);
        item.SetMetadata("FileHash", file.hash);
        return (ITaskItem)item;
    }).ToArray();

    internal static string Fingerprint(IReadOnlyList<RuntimeFile> files) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
        string.Join('\n', files.Select(static file => file.relative + "|" + file.hash)))));

    internal static ITaskItem[] Publish(
        string cache,
        ITaskItem[] inputs,
        IBuildEngine4 engine,
        string load,
        CancellationToken cancellation
    ) {
        RuntimeFile[] files = FreezeFiles(inputs);
        string fingerprint = Fingerprint(files);
        string destination = Path.Combine(cache, fingerprint);
        var publisher = new BuildCachePublisher(cache, BuildCacheArtifactRole.ImmutableArtifact);
        using FileLease activity = publisher.AcquireUseAsync(cancellation).AsTask().GetAwaiter().GetResult();
        using FileLease lease = FileLease.AcquireAsync(destination + ".lock", Timeout.InfiniteTimeSpan, cancellation)
            .AsTask().GetAwaiter().GetResult();
        ValidateSources(files, cancellation);
        if (!IsComplete(Path.Combine(destination, "Runtime"), files, cancellation))
            PublishCandidate(destination, files, cancellation);
        ValidateSources(files, cancellation);
        ITaskItem[] output = files.Select(file =>
        {
            var item = new TaskItem(Path.Combine(destination, "Runtime", file.relative));
            item.SetMetadata("RelativePath", file.relative);
            item.SetMetadata("FileHash", file.hash);
            return (ITaskItem)item;
        }).ToArray();
        publisher.Record(destination, destination + ".lock", [], null, null, cancellation);
        RegisterReader(load, Path.Combine(destination, "Runtime", "Inno.Build.Tasks.dll"));
        TaskRuntimeBuildScope.Register(engine, "publication:" + cache + "/" + fingerprint, output);
        return output;
    }

    internal static RuntimeFile[] FreezeFiles(ITaskItem[] inputFiles)
    {
        if (inputFiles.Length == 0)
            throw new InvalidDataException("The task runtime closure is empty.");
        var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        RuntimeFile[] files = inputFiles.Select(item =>
        {
            string relative = item.GetMetadata("RelativePath").Replace('\\', '/');
            string hash = item.GetMetadata("FileHash").ToUpperInvariant();
            if (relative.Length == 0 || relative.Split('/').Any(static segment => segment is "" or "." or "..")
                || relative.Contains(':') || !paths.Add(relative)
                || hash.Length != 64 || hash.Any(static character => !Uri.IsHexDigit(character)))
                throw new InvalidDataException("A runtime input has an invalid path, hash or duplicate identity.");
            return new RuntimeFile(RequireAbsolute(item.ItemSpec), relative, hash);
        }).OrderBy(static file => file.relative, StringComparer.Ordinal).ToArray();
        if (!paths.Contains("Inno.Build.Tasks.dll"))
            throw new InvalidDataException("The task runtime closure has no task assembly.");
        return files;
    }

    internal static string RequireAbsolute(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("Task host locations must be absolute.", nameof(path));
        return Path.GetFullPath(path);
    }

    internal static void ValidateSources(
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        foreach (RuntimeFile file in files)
        {
            cancellation.ThrowIfCancellationRequested();
            if (Hash(file.source) != file.hash)
                throw new InvalidDataException($"Task runtime input changed after selection: {file.source}");
        }
    }

    internal static bool IsComplete(
        string runtime,
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        if (!Directory.Exists(runtime))
            return false;
        string[] actual = PathBoundary.EnumerateFiles(runtime)
            .Select(path => Path.GetRelativePath(runtime, path).Replace('\\', '/')).ToArray();
        if (!actual.ToHashSet(StringComparer.Ordinal).SetEquals(files.Select(static file => file.relative)))
            return false;
        foreach (RuntimeFile file in files)
        {
            cancellation.ThrowIfCancellationRequested();
            if (Hash(PathBoundary.Resolve(runtime, file.relative)) != file.hash)
                return false;
        }
        return true;
    }

    internal static void PublishCandidate(
        string destination,
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        using ProcessOwnedDirectory candidateOwner = ProcessOwnedDirectory.CreateAsync(Path.GetDirectoryName(destination)!, cancellation)
            .AsTask().GetAwaiter().GetResult();
        string staging = candidateOwner.directory;
        string runtime = Path.Combine(staging, "Runtime");
        try
        {
            foreach (RuntimeFile file in files)
            {
                cancellation.ThrowIfCancellationRequested();
                string output = PathBoundary.Resolve(runtime, file.relative);
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                File.Copy(file.source, output);
            }
            if (!IsComplete(runtime, files, cancellation))
                throw new InvalidDataException("The copied task runtime did not match its selected closure.");
            ValidateSources(files, cancellation);
            cancellation.ThrowIfCancellationRequested();
            try
            {
                AtomicDirectory.Install(staging, destination);
            }
            catch (DirectoryCleanupException cleanupFailure)
            {
                Console.Error.WriteLine(cleanupFailure.Message);
            }
        }
        finally
        {
            candidateOwner.Dispose();
        }
    }

    internal static string Hash(string path)
    {
        using FileStream input = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(input));
    }

    internal static void RegisterReader(
        string load,
        string assembly
    ) {
        RegisterProcessOwner(load);
        Directory.CreateDirectory(load);
        AtomicFile.WriteAllBytes(Path.Combine(load, "source-host.txt"), Encoding.UTF8.GetBytes(assembly));
        string operation = Path.GetDirectoryName(load)!;
        string root = Path.GetDirectoryName(operation)!;
        string host = Path.GetDirectoryName(Path.GetDirectoryName(assembly))!;
        var hostPublisher = new BuildCachePublisher(Path.GetDirectoryName(host)!, BuildCacheArtifactRole.ImmutableArtifact);
        using FileLease activity = hostPublisher.AcquireUseAsync().AsTask().GetAwaiter().GetResult();
        new BuildCachePublisher(root, BuildCacheArtifactRole.ProcessWorkingDirectory).RecordProcessUse(operation, operation + ".lock", [host]);
    }

    internal static void RegisterProcessOwner(string directory)
    {
        string operation = Path.GetDirectoryName(directory)!;
        new BuildCachePublisher(Path.GetDirectoryName(operation)!, BuildCacheArtifactRole.ProcessWorkingDirectory).RecordProcessUse(operation, operation + ".lock", []);
    }

    internal sealed record RuntimeFile(
        string source,
        string relative,
        string hash
    );
}
