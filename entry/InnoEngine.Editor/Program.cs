using System;
using System.Reflection;
using System.Threading;
using Inno.Core.IO;
#if INNO_HOST_WINDOWS_X64
using Inno.Editor.WindowsX64;
#elif INNO_HOST_MACOS_ARM64
using Inno.Editor.MacOSArm64;
#endif

namespace InnoEngine.Editor;

internal static class Program
{
    private static int Main(string[] arguments)
    {
        using var cancellation = new CancellationTokenSource();
        ConsoleCancelEventHandler onCancel = (
            _,
            options
        ) =>
        {
            options.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += onCancel;
        try
        {
            string ownership = ReadProductLock();
            // Product clean must wait until the running Editor has released its deployment.
            if (!System.IO.File.Exists(ownership))
                FileLease.AcquireAsync(ownership, TimeSpan.FromSeconds(10), cancellation.Token)
                    .GetAwaiter().GetResult().Dispose();
            using FileLease reading = FileLease.AcquireSharedAsync(ownership, TimeSpan.FromSeconds(10), cancellation.Token)
                .GetAwaiter().GetResult();
#if INNO_HOST_WINDOWS_X64
            return WindowsX64EditorComposition.Run(arguments, cancellation.Token);
#elif INNO_HOST_MACOS_ARM64
            return MacOSArm64EditorComposition.Run(arguments, cancellation.Token);
#else
            throw new PlatformNotSupportedException("No Editor is implemented for this SDK host.");
#endif
        }
        catch (OperationCanceledException)
        {
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
        finally
        {
            Console.CancelKeyPress -= onCancel;
        }
    }

    private static string ReadProductLock()
    {
        foreach (AssemblyMetadataAttribute metadata in typeof(Program).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
            if (metadata.Key == "Inno.Editor.ProductLock" && !string.IsNullOrWhiteSpace(metadata.Value))
                return metadata.Value;
        throw new InvalidOperationException("The Editor build did not declare its product ownership path.");
    }
}
