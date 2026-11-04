using System;

using Inno.Native.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Controls independent context time without synthesizing input.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    /// <inheritdoc />
    public UiContextTime GetTime(UiContextHandle context)
    {
        EnsureActive();
        double seconds = 0;
        byte paused = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetTime(
            m_runtime, Require(context), &seconds, &paused, &result, &error), error);
        ThrowIfFailed(result, "query UI time");
        return new UiContextTime(seconds, paused != 0);
    }

    /// <inheritdoc />
    public void SetTimePaused(
        UiContextHandle context,
        bool paused
    ) {
        EnsureActive();
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.SetTimePaused(
            m_runtime, Require(context), paused ? (byte)1 : (byte)0, &result, &error), error);
        ThrowIfFailed(result, "pause UI time");
    }

    /// <inheritdoc />
    public void AdvanceTime(
        UiContextHandle context,
        double seconds
    ) {
        EnsureActive();
        ValidateTime(seconds);
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.AdvanceTime(
            m_runtime, Require(context), seconds, &result, &error), error);
        if (result == Result.InvalidArgument)
            throw new ArgumentOutOfRangeException(nameof(seconds), "The step would overflow context time.");
        ThrowIfFailed(result, "advance UI time");
    }

    /// <inheritdoc />
    public bool TrySeekTime(
        UiContextHandle context,
        double seconds
    ) {
        ValidateTime(seconds);
        _ = GetTime(context);
        return false;
    }

    /// <inheritdoc />
    public void RefreshLayout(UiContextHandle context)
    {
        EnsureActive();
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.RefreshLayout(
            m_runtime, Require(context), &result, &error), error);
        ThrowIfFailed(result, "refresh UI layout");
    }

    private static void ValidateTime(double seconds)
    {
        if (!double.IsFinite(seconds) || seconds < 0)
            throw new ArgumentOutOfRangeException(nameof(seconds), "UI time must be finite and nonnegative.");
    }
}
