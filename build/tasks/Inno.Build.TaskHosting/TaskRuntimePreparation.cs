using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Inno.Core.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Inno.Build.TaskHosting;

internal static class TaskRuntimePreparation
{
    internal static string Fingerprint(IReadOnlyList<RuntimeFile> files) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
        string.Join('\n', files.Select(static file => file.relative + "|" + file.hash)))));

    internal static ITaskItem[] Publish(
        string cache,
        ITaskItem[] inputs,
        IBuildEngine4 engine,
        CancellationToken cancellation
    ) {
        RuntimeFile[] files = FreezeFiles(inputs);
        string fingerprint = Fingerprint(files);
        string destination = Path.Combine(cache, fingerprint);
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
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
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
            AtomicDirectory.Install(staging, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
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
    }

    internal static void RegisterProcessOwner(string directory)
    {
        string operation = Path.GetDirectoryName(directory)!;
        string owners = Path.Combine(operation, "owners");
        Directory.CreateDirectory(owners);
        string process = Environment.ProcessId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        AtomicFile.WriteAllBytes(Path.Combine(owners, process + ".pid"), Encoding.UTF8.GetBytes(process));
    }

    internal sealed record RuntimeFile(
        string source,
        string relative,
        string hash
    );
}
