using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Inno.Core.IO;

internal static partial class PosixFilePublication
{
    internal static void Install(
        string source,
        string destination
    ) {
        int result;
        try
        {
            result = Link(source, destination);
        }
        catch (Exception exception) when (exception is DllNotFoundException or EntryPointNotFoundException or PlatformNotSupportedException)
        {
            throw new PlatformNotSupportedException("The host does not provide atomic create-only file publication through POSIX link.", exception);
        }
        if (result != 0)
        {
            int error = Marshal.GetLastPInvokeError();
            throw new IOException(
                $"Atomic create-only publication of '{source}' at '{destination}' failed (errno {error}). " +
                "The destination was not replaced; the filesystem must support same-directory hard links.",
                new Win32Exception(error));
        }

        try
        {
            File.Delete(source);
        }
        catch (IOException exception)
        {
            throw CleanupFailure(source, destination, exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw CleanupFailure(source, destination, exception);
        }
    }

    private static IOException CleanupFailure(
        string source,
        string destination,
        Exception exception
    ) => new($"The complete file was published at '{destination}', but its candidate '{source}' could not be retired.", exception);

    [LibraryImport("libc", EntryPoint = "link", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    private static partial int Link(
        string source,
        string destination
    );
}
