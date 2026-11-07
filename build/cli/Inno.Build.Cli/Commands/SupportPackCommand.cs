using System;
using System.IO;
using System.IO.Compression;
using Inno.Build.SupportPacks;
using Inno.Core.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Cli;

internal static class SupportPackCommand
{
    internal static async Task RunAsync(
        BuildCommandContext context,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string output = options.Read("output", Path.Combine(context.root, "artifacts", "support-packs"));
        using PlayerSupportPackPublication pack = await context.distribution.supportPacks.PublishAsync(context.root, output, context.target,
            context.composition.dotnetHost, cancellationToken).ConfigureAwait(false);
        string archive = options.Read("archive", string.Empty);
        if (archive.Length != 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // The publication retains reading rights throughout export; no lease or mutable catalog index is copied.
            string destination = Path.GetFullPath(archive);
            string candidate = destination + ".staging-" + Guid.NewGuid().ToString("N");
            try
            {
                using (var stream = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    ZipFile.CreateFromDirectory(pack.directory, stream, CompressionLevel.Fastest, includeBaseDirectory: false);
                    stream.Flush(flushToDisk: true);
                }
                cancellationToken.ThrowIfCancellationRequested();
                AtomicFile.Install(candidate, destination);
            }
            finally
            {
                if (File.Exists(candidate))
                    File.Delete(candidate);
            }
        }
        Console.WriteLine(pack.directory);
        foreach (string diagnostic in pack.diagnostics)
            Console.Error.WriteLine(diagnostic);
    }
}
