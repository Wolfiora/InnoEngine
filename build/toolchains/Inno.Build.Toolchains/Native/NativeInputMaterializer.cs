using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Materializes frozen native inputs without rewriting identical intermediate files.
/// </summary>
public static class NativeInputMaterializer
{
    /// <summary>
    /// Copies a declared input atomically when its destination differs, preserving identical file timestamps.
    /// </summary>
    /// <param name="context">
    /// The operation whose initial source bytes and hashing statistics are shared across components.
    /// </param>
    /// <param name="source">
    /// The absolute input file already declared by the component recipe.
    /// </param>
    /// <param name="destination">
    /// The absolute intermediate file owned exclusively by the component producer.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels copying before publication and preserves the prior intermediate file.
    /// </param>
    /// <returns>
    /// True when bytes were copied and published; false when the existing content was identical.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The source changes relative to the operation's frozen input snapshot.
    /// </exception>
    /// <exception cref="IOException">
    /// The source, destination or atomic candidate cannot be read or published.
    /// </exception>
    public static async ValueTask<bool> CopyAsync(
        NativeBuildContext context,
        string source,
        string destination,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        NativeBuildInput input = NativeBuildInput.FromPath(context.engineRoot, source);
        NativeBuildInput output = new(input.logicalPath, destination);
        cancellationToken.ThrowIfCancellationRequested();
        byte[] expected = context.inputState.GetFrozenHash(input.physicalPath);
        if (File.Exists(destination))
        {
            byte[] existing = context.inputState.CaptureVerification([output], cancellationToken).entries.Single().hash;
            if (expected.AsSpan().SequenceEqual(existing))
                return false;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(output.physicalPath)!);
        string candidate = output.physicalPath + ".staging-" + Guid.NewGuid().ToString("N");
        try
        {
            await using (FileStream read = File.OpenRead(input.physicalPath))
            await using (FileStream write = new(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                64 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await read.CopyToAsync(write, cancellationToken).ConfigureAwait(false);
                await write.FlushAsync(cancellationToken).ConfigureAwait(false);
                write.Flush(flushToDisk: true);
            }
            byte[] copied = context.inputState.CaptureVerification(
                [new NativeBuildInput(input.logicalPath, candidate)], cancellationToken).entries.Single().hash;
            if (!expected.AsSpan().SequenceEqual(copied))
                throw new InvalidOperationException($"Native input '{input.logicalPath}' changed during materialization.");
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(candidate, File.GetUnixFileMode(input.physicalPath));
            cancellationToken.ThrowIfCancellationRequested();
            AtomicFile.Install(candidate, output.physicalPath);
            context.inputState.RecordMaterialization(new FileInfo(output.physicalPath).Length);
            return true;
        }
        finally
        {
            if (File.Exists(candidate))
                File.Delete(candidate);
        }
    }
}
