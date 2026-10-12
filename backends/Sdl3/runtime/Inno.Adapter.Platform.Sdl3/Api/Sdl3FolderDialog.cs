using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Opens an SDL native directory dialog while the borrowed application pumps platform events.
/// </summary>
public static class Sdl3FolderDialog
{
    /// <summary>
    /// Selects one directory before a product creates its Editor session or rendering device.
    /// </summary>
    /// <param name="application">
    /// A live application borrowed on its owner thread for the entire modal operation.
    /// </param>
    /// <returns>
    /// The selected directory, or null when the user cancels. No project is created or modified.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The application is absent.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// SDL reports a native dialog failure or the application is used outside its owner thread.
    /// </exception>
    public static unsafe string? SelectFolder(Sdl3PlatformApplication application)
    {
        ArgumentNullException.ThrowIfNull(application);
        application.GetWindows();
        var result = new TaskCompletionSource<string?>(TaskCreationOptions.RunContinuationsAsynchronously);
        GCHandle callbackOwner = GCHandle.Alloc(result);
        try
        {
            SDL.ShowOpenFolderDialog(&Complete, (void*)GCHandle.ToIntPtr(callbackOwner), default, null, 0);
            Exception? pumpFailure = null;
            while (!result.Task.IsCompleted)
            {
                if (pumpFailure is null)
                {
                    try
                    {
                        while (application.PollEvent(out _)) { }
                    }
                    catch (Exception failure)
                    {
                        // The native dialog still owns the callback until completion.
                        pumpFailure = failure;
                    }
                }
                Thread.Sleep(10);
            }
            if (pumpFailure is not null)
                throw new InvalidOperationException("Platform event processing failed while selecting a directory.", pumpFailure);
            return result.Task.GetAwaiter().GetResult();
        }
        finally
        {
            callbackOwner.Free();
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Complete(
        void* userData,
        byte** files,
        int filter
    ) {
        var result = (TaskCompletionSource<string?>)GCHandle.FromIntPtr((nint)userData).Target!;
        try
        {
            if (files == null)
                result.TrySetException(new InvalidOperationException(SDL.GetError() ?? "The native folder dialog failed."));
            else
                result.TrySetResult(files[0] == null ? null : Marshal.PtrToStringUTF8((nint)files[0]));
        }
        catch (Exception failure)
        {
            result.TrySetException(failure);
        }
    }
}
