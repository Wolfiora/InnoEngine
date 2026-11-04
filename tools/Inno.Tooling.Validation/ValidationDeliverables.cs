using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal static class ValidationDeliverables
{
    internal static void Create(
        ValidationPlan plan,
        ValidationCheck check,
        CancellationToken cancellation
    ) {
        JsonElement editor = plan.contexts[check.context!].sdkGraph.EnumerateArray().Single(snapshot =>
            Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner);
        string source = Path.GetDirectoryName(editor.GetProperty("Properties").GetProperty("TargetPath").GetString()!)!;
        string directory = Path.Combine(plan.artifacts, "deliverables");
        string destination = Path.Combine(directory, "Editor.zip");
        string candidate = destination + ".staging-" + Guid.NewGuid().ToString("N");
        try
        {
            using (var stream = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                    foreach (string file in PathBoundary.EnumerateFiles(source).Order(StringComparer.Ordinal))
                    {
                        cancellation.ThrowIfCancellationRequested();
                        ZipArchiveEntry entry = archive.CreateEntry(Path.GetRelativePath(source, file).Replace('\\', '/'), CompressionLevel.Fastest);
                        entry.LastWriteTime = new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero);
                        if (!OperatingSystem.IsWindows())
                            entry.ExternalAttributes = ((int)File.GetUnixFileMode(file) | 0x8000) << 16;
                        using Stream output = entry.Open();
                        using FileStream input = File.OpenRead(file);
                        input.CopyTo(output);
                    }
                stream.Flush(flushToDisk: true);
            }
            cancellation.ThrowIfCancellationRequested();
            AtomicFile.Install(candidate, destination);
        }
        finally
        {
            if (File.Exists(candidate))
                File.Delete(candidate);
        }
        var files = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string name in new[] { "Editor.zip", "PlayerSupportPack.zip" })
        {
            cancellation.ThrowIfCancellationRequested();
            using FileStream stream = File.OpenRead(Path.Combine(directory, name));
            files.Add(name, Convert.ToHexStringLower(SHA256.HashData(stream)));
        }
        AtomicFile.WriteAllBytes(Path.Combine(directory, "deliverable-manifest.json"), JsonSerializer.SerializeToUtf8Bytes(new
        {
            format = 1,
            plan.commit,
            plan.tree,
            plan.environment,
            planIdentity = ValidationInputs.PlanIdentity(plan),
            configuration = plan.contexts[check.context!].properties["Configuration"],
            platform = Environment.GetEnvironmentVariable("VALIDATION_PLATFORM") ?? "local",
            files
        }, new JsonSerializerOptions { WriteIndented = true }));
    }
}
