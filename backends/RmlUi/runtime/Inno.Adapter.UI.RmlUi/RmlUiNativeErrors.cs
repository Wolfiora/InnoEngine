using System;
using System.Runtime.InteropServices;
using Inno.Native.UI;

namespace Inno.Adapter.UI.RmlUi;

internal static unsafe class RmlUiNativeErrors
{
    internal static InnoUIRmlUiAdapterRuntime* CreateSession()
    {
        InnoUIRmlUiAdapterRuntime* runtime = null;
        BgcsInnoRmluiError* error = null;
        CheckBridge(UiNative.Raw.CreateRuntime(&runtime, &error), error);
        return runtime;
    }

    internal static void DestroySession(InnoUIRmlUiAdapterRuntime* runtime)
    {
        BgcsInnoRmluiError* error = null;
        CheckBridge(UiNative.Raw.DestroyRuntime(runtime, &error), error);
    }

    internal static void CheckBridge(
        int status,
        BgcsInnoRmluiError* error
    ) {
        if (status == 0 && error == null)
            return;
        try
        {
            string? message = error == null ? null : Marshal.PtrToStringUTF8((nint)UiNative.Raw.BgcsInnoRmluiErrorMessage(error));
            throw new InvalidOperationException($"The UI bridge failed with status {status}: {message ?? "No native error text is available."}");
        }
        finally
        {
            if (error != null)
                UiNative.Raw.BgcsInnoRmluiErrorRelease(error);
        }
    }
}
